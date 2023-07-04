using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using EmailAPI.Message;
using EmailAPI.Models.Dto;
using EmailAPI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly EmailService _emailService;
        private readonly ServiceBusProcessor _emailOrderPlacedProcessor;
        private readonly ServiceBusProcessor _emailCartProcessor;
        private readonly ServiceBusProcessor _registerUserProcessor;
        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;

            var serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            var emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            var registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
            var orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            var orderCreatedEmailSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _registerUserProcessor = client.CreateProcessor(registerUserQueue);
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedEmailSubscription);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.StopProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //TODO - try to log email
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            //this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                //TODO - try to log email
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //TODO - try to log email
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}

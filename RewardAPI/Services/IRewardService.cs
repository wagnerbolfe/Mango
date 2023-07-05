﻿using System.Threading.Tasks;
using RewardAPI.Message;

namespace RewardAPI.Services
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}

using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IProfileService : IDisposable
    {
        int AddProfile(ProfileDTO profile);
        ProfileDTO GetProfileById(int profileId);
        ProfileDTO GetProfileByUser(string userId);
        int UpdateOrCreateProfile(ProfileDTO profile);
        bool DeleteProfileById(int profileId);
        HashSet<ProfileDTO> GetProfileSubscriptions(int profileId);
        HashSet<ProfileDTO> GetProfileSubscribers(int profileId);
        HashSet<ProfileDTO> GetProfilesByName(string profileName);
        HashSet<ProfileDTO> GetAllProfiles();
        void AddSubscription(int subscriptionProfileId, int profileId);
        void PutSubscription(int subscriptionProfileId, int profileId);
        bool RemoveProfileSubscriber(int subscriberProfileId, int profileId);
        bool RemoveProfileSubscription(int subscriptionProfileId, int profileId);
    }
}

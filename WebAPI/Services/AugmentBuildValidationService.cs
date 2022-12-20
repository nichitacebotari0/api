using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Services
{
    public class AugmentBuildValidationService : IAugmentBuildValidationService
    {
        private readonly ApplicationDbContext applicationDbContext;

        private const int COMBAT_CATEGORY = 2;
        private const int UTILITY_CATEGORY = 3;
        private const int ULTIMATE_CATEGORY = 4;
        public AugmentBuildValidationService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task Validate(BuildViewModel buildViewModel)
        {
            int[] augmentIds = buildViewModel.Augments.Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x =>
                {
                    if (!int.TryParse(x, out int result))
                        throw new ArgumentException("Not a valid augment id: " + x);
                    return result;
                })
                .ToArray();

            if (augmentIds.Length != 8)
                throw new ArgumentException("Build needs to have 8 augments(even if some are not filled: -1)");

            var heroAugmentModels = await applicationDbContext.Augment
                .Where(x => augmentIds.Contains(x.Id)).ToArrayAsync();
            var artifactModels = await applicationDbContext.Artifact
                .Where(x => augmentIds.Contains(x.Id)).ToArrayAsync();
            var boonModels = await applicationDbContext.Active
                .Where(x => augmentIds.Contains(x.Id)).ToArrayAsync();

            var heroAugments = augmentIds.Select(x => heroAugmentModels.FirstOrDefault(aug => aug.Id == x)).ToArray();
            if (heroAugments.Any(augment => augment != null && augment.HeroId != buildViewModel.HeroId))
                throw new ArgumentException("Build contains augments for another hero");

            ValidateArtifact(augmentIds[0], artifactModels);
            ValidateCombat(augmentIds[1], heroAugmentModels);
            ValidateUtility(augmentIds[2], heroAugmentModels);
            ValidateFlex(augmentIds[3], heroAugmentModels, artifactModels);
            ValidateUltimate(augmentIds[4], heroAugmentModels);
            ValidateBoon(augmentIds[5], boonModels);
            ValidateFlex(augmentIds[6], heroAugmentModels, artifactModels);
            ValidateFlex(augmentIds[7], heroAugmentModels, artifactModels);
        }

        private void ValidateFlex(int id, Augment[] heroAugmentModels, Artifact[] artifactModels)
        {
            if (id < 0)
                return;

            var artifactModel = artifactModels.FirstOrDefault(x => x.Id == id);
            var augmentModel = heroAugmentModels.FirstOrDefault(x => x.Id == id);

            if (artifactModel == null && augmentModel == null)
                throw new ArgumentException($"Augment with id {id} is not a FLEX augment");

            if (artifactModel != null)
            {
                ValidateArtifact(id, artifactModels);
                return;
            }

            switch (augmentModel.AugmentCategoryId)
            {
                case COMBAT_CATEGORY:
                    ValidateCombat(id, heroAugmentModels);
                    break;
                case UTILITY_CATEGORY:
                    ValidateUtility(id, heroAugmentModels);
                    break;
                default:
                    throw new ArgumentException($"Augment with id {id} is neither COMBAT nor UTILITY");
            }
        }

        private void ValidateBoon(int id, Active[] boonModels)
        {
            if (id < 0)
                return;

            var augmentModel = boonModels.FirstOrDefault(x => x.Id == id);
            if (augmentModel == null)
                throw new ArgumentException($"Active Boon with id {id} doesn't exist");
        }

        private void ValidateUltimate(int id, Augment[] heroAugmentModels)
        {
            if (id < 0)
                return;

            var augmentModel = heroAugmentModels.FirstOrDefault(x => x.Id == id);
            if (augmentModel == null)
                throw new ArgumentException($"Augment with id {id} doesn't exist");

            if (augmentModel.AugmentCategoryId != ULTIMATE_CATEGORY)
                throw new ArgumentException($"Augment with id {id} is not an ULTIMATE augment");
        }

        private void ValidateUtility(int id, Augment[] heroAugmentModels)
        {
            if (id < 0)
                return;

            var augmentModel = heroAugmentModels.FirstOrDefault(x => x.Id == id);
            if (augmentModel == null)
                throw new ArgumentException($"Augment with id {id} doesn't exist");

            if (augmentModel.AugmentCategoryId != UTILITY_CATEGORY)
                throw new ArgumentException($"Augment with id {id} is not a UTILITY augment");
        }

        private void ValidateCombat(int id, Augment[] heroAugmentModels)
        {
            if (id < 0)
                return;

            var augmentModel = heroAugmentModels.FirstOrDefault(x => x.Id == id);
            if (augmentModel == null)
                throw new ArgumentException($"Augment with id {id} doesn't exist");

            if (augmentModel.AugmentCategoryId != COMBAT_CATEGORY)
                throw new ArgumentException($"Augment with id {id} is not a COMBAT augment");
        }

        private void ValidateArtifact(int id, Artifact[] artifactModels)
        {
            if (id < 0)
                return;

            var artifactModel = artifactModels.FirstOrDefault(x => x.Id == id);
            if (artifactModel == null)
                throw new ArgumentException($"Artifact with id {id} doesn't exist");
        }
    }
}

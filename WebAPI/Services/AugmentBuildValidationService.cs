using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Services
{
    public class AugmentBuildValidationService : IAugmentBuildValidationService
    {
        private readonly ApplicationDbContext applicationDbContext;

        private const int ARFTIFACT_CATEGORY = 1;
        private const int COMBAT_CATEGORY = 2;
        private const int UTILITY_CATEGORY = 3;
        private const int ULTIMATE_CATEGORY = 4;
        public AugmentBuildValidationService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task Validate(BuildViewModel buildViewModel)
        {
            var augments = buildViewModel.Augments.Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x =>
                {
                    if (!int.TryParse(x.Split(':')[0], out int category))
                        throw new ArgumentException("Not a valid category id: " + x);
                    if (!int.TryParse(x.Split(':')[1], out int augment))
                        throw new ArgumentException("Not a valid augment id: " + x);
                    return (category, augment);
                })
                .ToArray();

            if (augments.Length != 8)
                throw new ArgumentException("Build needs to have 8 augments(even if some are not filled: -1)");

            var heroAugmentModels = await applicationDbContext.Augment
                .Where(x => augments.Select(x => x.augment).Contains(x.Id)).ToArrayAsync();
            var artifactModels = await applicationDbContext.Artifact
                .Where(x => augments.Select(x => x.augment).Contains(x.Id)).ToArrayAsync();
            var boonModels = await applicationDbContext.Active
                .Where(x => augments.Select(x => x.augment).Contains(x.Id)).ToArrayAsync();

            var heroCategories = new[] { COMBAT_CATEGORY, UTILITY_CATEGORY, ULTIMATE_CATEGORY };
            var heroAugments = augments
                .Where(x => heroCategories.Contains(x.category))
                .Select(x => heroAugmentModels.FirstOrDefault(aug => aug.Id == x.augment))
                .ToArray();

            if (heroAugments.Any(augment => augment != null && augment.HeroId != buildViewModel.HeroId))
                throw new ArgumentException("Build contains augments for another hero");

            ValidateArtifact(augments[0].augment, artifactModels);
            ValidateCombat(augments[1].augment, heroAugmentModels);
            ValidateUtility(augments[2].augment, heroAugmentModels);
            ValidateFlex(augments[3], heroAugmentModels, artifactModels);
            ValidateUltimate(augments[4].augment, heroAugmentModels);
            ValidateBoon(augments[5].augment, boonModels);
            ValidateFlex(augments[6], heroAugmentModels, artifactModels);
            ValidateFlex(augments[7], heroAugmentModels, artifactModels);
        }

        private void ValidateFlex((int category, int id) augment, Augment[] heroAugmentModels, Artifact[] artifactModels)
        {
            if (augment.id < 0)
                return;

            switch (augment.category)
            {
                case ARFTIFACT_CATEGORY:
                    ValidateArtifact(augment.id, artifactModels);
                    break;
                case COMBAT_CATEGORY:
                    ValidateCombat(augment.id, heroAugmentModels);
                    break;
                case UTILITY_CATEGORY:
                    ValidateUtility(augment.id, heroAugmentModels);
                    break;
                default:
                    throw new ArgumentException($"Augment with id {augment.id} is neither COMBAT nor UTILITY");
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

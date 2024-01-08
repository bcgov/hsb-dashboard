import { IOrganizationModel, IServerItemModel } from '@/hooks';
import { IOrganizationStorageModel } from './IOrganizationStorageModel';
import { SortOptions } from './sortOptions';

export const calcOrganizationStorage = (
  organizations: IOrganizationModel[],
  serverItems: IServerItemModel[],
  sort: SortOptions,
): IOrganizationStorageModel[] => {
  return organizations
    .map((org) => {
      const values = serverItems
        .filter((si) => si.organizationId === org.id)
        .map((si) => ({ capacity: si.capacity ?? 0, availableSpace: si.availableSpace ?? 0 }))
        .reduce(
          (a, b) => ({
            capacity: a.capacity + b.capacity,
            availableSpace: a.availableSpace + b.availableSpace,
          }),
          { capacity: 0, availableSpace: 0 },
        );
      const capacity = values.capacity;
      const availableSpace = values.availableSpace;
      const percentage = capacity ? Math.round(((capacity - availableSpace) / capacity) * 100) : 0;
      return { ...org, capacity, availableSpace, percentage };
    })
    .sort((a, b) => {
      switch (sort) {
        case SortOptions.LowestAvailableSpace:
          return a.availableSpace < b.availableSpace
            ? -1
            : a.availableSpace > b.availableSpace
            ? 1
            : 0;
        case SortOptions.HighestAvailableSpace:
        default:
          return a.availableSpace > b.availableSpace
            ? -1
            : a.availableSpace < b.availableSpace
            ? 1
            : 0;
      }
    });
};

import { IOption } from '@/components';

export enum SortOptions {
  HighestAvailableSpace = 0,
  LowestAvailableSpace = 1,
}

export const sortOptions: IOption<number>[] = [
  { label: 'Highest unused space', value: SortOptions.HighestAvailableSpace },
  { label: 'Highest used space', value: SortOptions.LowestAvailableSpace },
];

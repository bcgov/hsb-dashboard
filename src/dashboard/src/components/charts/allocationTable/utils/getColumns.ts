export const getColumns = (showTenant: boolean) => {
  let dropdownConfigs = [
    {
      label: 'Server',
      sort: 'server',
      options: [
        { label: 'A to Z', value: 'asc' },
        { label: 'Z to A', value: 'desc' },
      ],
    },
    {
      label: 'OS Version',
      sort: 'os',
      options: [
        { label: 'Latest', value: 'desc' },
        { label: 'Oldest', value: 'asc' },
      ],
    },
    {
      label: 'Allocated Space',
      sort: 'capacity',
      options: [
        { label: 'Smallest', value: 'asc' },
        { label: 'Largest', value: 'desc' },
      ],
    },
    {
      label: 'Unused',
      sort: 'available',
      options: [
        { label: 'Smallest', value: 'asc' },
        { label: 'Largest', value: 'desc' },
      ],
    },
  ];

  if (showTenant) {
    dropdownConfigs.splice(1, 0, {
      label: 'Tenant',
      sort: 'tenant',
      options: [
        { label: 'A to Z', value: 'asc' },
        { label: 'Z to A', value: 'desc' },
      ],
    });
  }

  return dropdownConfigs;
};

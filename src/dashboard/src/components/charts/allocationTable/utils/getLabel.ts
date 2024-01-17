export const getLabel = (className: string) => {
  switch (className) {
    case 'cmdb_ci_aix_server':
      return 'AIX Servers';
    case 'cmdb_ci_esx_server':
      return 'ESX Servers';
    case 'cmdb_ci_server':
      return 'Servers';
    case 'cmdb_ci_win_server':
      return 'Windows Servers';
    case 'cmdb_ci_linux_server':
      return 'Linux Servers';
    case 'cmdb_ci_unix_server':
      return 'Unix Servers';
    case 'cmdb_ci_solaris_server':
      return 'Solaris Servers';
    case 'cmdb_ci_pc_hardware':
      return 'PC Hardware';
    case 'u_openvms':
      return 'VMS';
    case 'u_cmdb_ci_appliance':
      return 'Appliance';
    default:
      return 'Unknown';
  }
};

import { OperatingSystems } from '../constants';

export const getLabel = (operatingSystem: OperatingSystems) => {
  switch (operatingSystem) {
    case OperatingSystems.AIX:
      return 'AIX Servers';
    case OperatingSystems.ESX:
      return 'ESX Servers';
    case OperatingSystems.Server:
      return 'Servers';
    case OperatingSystems.Windows:
      return 'Windows Servers';
    case OperatingSystems.Linux:
      return 'Linux Servers';
    case OperatingSystems.Unix:
      return 'Unix Servers';
    case OperatingSystems.Solaris:
      return 'Solaris Servers';
    case OperatingSystems.PCHardware:
      return 'PC Hardware';
    case OperatingSystems.VMS:
      return 'VMS';
    default:
      return 'Unknown';
  }
};

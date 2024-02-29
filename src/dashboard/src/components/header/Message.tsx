import { useAppStore, useDashboardStore } from '@/store';
import { usePathname } from 'next/navigation';

export const Message = () => {
  const path = usePathname();
  const userInfo = useAppStore((state) => state.userinfo);
  const {
    organization,
    operatingSystemItem,
    serverItem,
  } = useDashboardStore((state) => ({
    organization: state.organization,
    operatingSystemItem: state.operatingSystemItem,
    serverItem: state.serverItem,
  }));
  const operatingSystemItems = useAppStore((state) => state.operatingSystemItems);

  const isDashboardView = path.includes('/dashboard');
  const isServerView = path.includes('/servers');
  const isHSBUserAdminView = path.includes('/hsb/admin/users');
  const isHSBOrganizationAdminView = path.includes('/hsb/admin/organizations');
  const isClientOrganizationAdminView = path.includes('/client/admin/organizations');

  if (isDashboardView) {
    if (!!organization && !!serverItem) {
      return (
        <p>
          Showing results for: {organization.name}, {serverItem.name}
        </p>
      );
    }

    if (serverItem) {
      var os = operatingSystemItems.find(
        (os) => os.id === serverItem?.operatingSystemItemId,
      );
      return (
        <p>
          Showing results for: {serverItem.name}
          {os ? `: ${os.name}` : ''}
        </p>
      );
    }

    if (!!organization && !!operatingSystemItem)
      return (
        <p>
          Showing results for: {organization.name}, all {operatingSystemItem.name}{' '}
          servers. <br />
          Use the filters to see further breakdowns of storage data.
        </p>
      );

    if (!!organization)
      return (
        <p>
          Showing results for: {organization.name}. <br />
          Use the filters to see further breakdowns of storage data.
        </p>
      );

    if (!!operatingSystemItem)
      return (
        <p>
          Showing results for: all {operatingSystemItem.name} servers. <br />
          Use the filters to see further breakdowns of storage data.
        </p>
      );

    return (
      <p>
        Welcome to the storage dashboard. This is an overview of the storage consumption for all
        organizations you belong to. <br />
        Use the filters to see further breakdowns of storage data.
      </p>
    );
  }

  if (isServerView) {
    return <p>Showing results for: All servers visible to {userInfo?.username}</p>;
  }

  if (isHSBUserAdminView) {
    return (
      <p>
        Welcome to the HSB User Management page. This page allows HSB administrators the ability to
        assign roles to users.
      </p>
    );
  }

  if (isHSBOrganizationAdminView) {
    return (
      <p>
        Welcome to the HSB Administrator Organization List page. This page provides HSB
        administrators with the ability to look up organizations to enable access to the dashboard.
      </p>
    );
  }

  if (isClientOrganizationAdminView) {
    return (
      <p>
        Welcome to the storage dashboard organization administration page. This page provides client
        administrators with the ability to self-manage other users within their organization(s).
      </p>
    );
  }

  return <></>;
};

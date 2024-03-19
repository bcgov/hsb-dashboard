import { dispatch } from '@/utils';
import React from 'react';

/**
 * Download configuration options interface.
 */
export interface IDownloadConfig extends RequestInit {
  fileName?: string;
}

/**
 * Make an AJAX request to download content from the specified endpoint.
 */
export const useDownload = () => {
  return React.useCallback(
    async (input: string | Request | URL, init?: IDownloadConfig | undefined) => {
      const response = await dispatch(input, {
        method: 'get',
        ...init,
      });

      const blob = await response.blob();
      const uri = window.URL.createObjectURL(new Blob([blob]));
      const link = document.createElement('a');
      link.href = uri;
      link.setAttribute('download', init?.fileName ?? `download-${new Date().toDateString()}`);
      document.body.appendChild(link);
      link.click();

      return response;
    },
    [],
  );
};

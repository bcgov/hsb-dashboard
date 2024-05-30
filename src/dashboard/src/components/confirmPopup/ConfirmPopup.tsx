'use client';

import { Button } from '@/components/buttons';
import { useNavigateStore } from '@/store';
import { useRouter } from 'next/navigation';
import React from 'react';
import styles from './ConfirmPopup.module.scss';

export interface PopupProps {
  show?: boolean;
  onCancel?: () => void;
  onConfirm?: () => void;
}

export const ConfirmPopup: React.FC<PopupProps> = ({
  show: initShow = false,
  onCancel,
  onConfirm,
}) => {
  const router = useRouter();
  const navigateTo = useNavigateStore((state) => state.navigateTo);
  const setEnableNavigate = useNavigateStore((state) => state.setEnableNavigate);
  const showNavConfirmation = useNavigateStore((state) => state.showNavConfirmation);
  const setShowNavConfirmation = useNavigateStore((state) => state.setShowNavConfirmation);

  React.useEffect(() => {
    setShowNavConfirmation(initShow);
  }, [initShow, setShowNavConfirmation]);

  if (!showNavConfirmation) return <></>;

  return (
    <>
      <span className={styles.backdrop}></span>
      <dialog className={styles.popupConfirm}>
        <p className={styles.popupTitle}>Confirm Navigation</p>
        <p className={styles.popupContent}>
          You have unsaved changes.
          <br />
          Are you sure you want to navigate away?
        </p>
        <div className={styles.popupFooter}>
          <Button
            variant="secondary"
            onClick={() => {
              setShowNavConfirmation(false);
              setEnableNavigate(true);
              onConfirm?.();
              if (navigateTo) {
                router.push(navigateTo.toString());
              }
            }}
          >
            Leave this page
          </Button>
          <Button
            onClick={() => {
              setShowNavConfirmation(false);
              onCancel?.();
            }}
          >
            Stay on this page
          </Button>
        </div>
      </dialog>
    </>
  );
};

import { Button } from '@/components';
import React from 'react';
import styles from './Dialog.module.scss';

export interface IDialogProps extends React.AllHTMLAttributes<HTMLDialogElement> {
  show?: boolean;
  header?: React.ReactElement;
  actions?: React.ReactNode;
  showCancel?: boolean;
  cancelLabel?: React.ReactNode;
  showActions?: boolean;
}

/**
 * Provides a dialog component with a default styling and cancel button.
 * returns Component
 */
export const Dialog = React.forwardRef<HTMLDialogElement, IDialogProps>(function Dialog(
  {
    show: initShow,
    header,
    actions,
    cancelLabel = 'Cancel',
    showCancel = true,
    showActions = true,
    children,
    ...rest
  },
  ref,
) {
  const dialogRef = React.useRef<HTMLDialogElement>(null);
  const [show, setShow] = React.useState(initShow);

  React.useImperativeHandle(ref, () => ({
    ...dialogRef.current!,
    showModal() {
      dialogRef.current?.showModal();
    },
    close() {
      dialogRef.current?.close();
    },
  }));

  React.useEffect(() => {
    setShow(initShow);
  }, [initShow]);

  React.useEffect(() => {
    if (show) dialogRef.current?.showModal();
    else dialogRef.current?.close();
  }, [show]);

  const closeDialog = () => {
    dialogRef.current?.close();
    setShow(false);
  };

  const Header = React.cloneElement(header ?? <></>, {
    className: `${styles.popupTitle} ${header?.props.className}`,
  });

  return (
    <dialog ref={dialogRef} className={styles.popup} {...rest}>
      {Header}
      {children}
      {showActions && (
        <div className={styles.popupFooter}>
          {showCancel && (
            <Button variant="secondary" onClick={closeDialog}>
              {cancelLabel}
            </Button>
          )}
          {actions}
        </div>
      )}
    </dialog>
  );
});

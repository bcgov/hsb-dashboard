import styles from './ConfirmPopup.module.scss';
import { Button } from '@/components/buttons';

export interface PopupProps {
    onCancel: () => void;
    onConfirm: () => void;
  }

export const ConfirmPopup: React.FC<PopupProps> = ({ onCancel, onConfirm }) => {
    return (
        <>
            <span className={styles.backdrop}></span>
            <dialog className={styles.popupConfirm}>
                <p className={styles.popupTitle}>Confirm Navigation</p>
                <p className={styles.popupContent}>You have unsaved changes.<br/>Are you sure you want to navigate away?</p>
                <div className={styles.popupFooter}>
                    <Button variant="secondary" onClick={onConfirm}>Leave this page</Button>
                    <Button onClick={onCancel}>Stay on this page</Button>
                </div>
            </dialog>
        </>
    )
};
  
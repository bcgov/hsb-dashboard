import OutlineBadge from '../OutlineBadge';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCloud, faComputer } from '@fortawesome/free-solid-svg-icons';

type ServerTypeBadgeProps = {
  isVirtual?: boolean;
};

export default function ServerTypeBadge({ isVirtual }: ServerTypeBadgeProps) {
  return (
    <OutlineBadge>
      {isVirtual ? (
        <>
          <FontAwesomeIcon style={{ marginRight: '2px' }} icon={faCloud} /> Virtual
        </>
      ) : (
        <>
          <FontAwesomeIcon style={{ marginRight: '2px' }} icon={faComputer} /> Physical
        </>
      )}
    </OutlineBadge>
  );
}

type OutlineBadgeProps = {
  children: React.ReactNode;
};

export default function OutlineBadge({ children }: OutlineBadgeProps) {
  return (
    <span
      style={{
        marginLeft: '10px',
        border: '1px solid #ccc',
        padding: '4px 8px',
        borderRadius: '4px',
      }}
    >
      {children}
    </span>
  );
}

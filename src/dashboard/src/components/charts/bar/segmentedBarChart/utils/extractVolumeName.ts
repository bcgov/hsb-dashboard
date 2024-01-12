export const extractVolumeName = (name?: string) => {
  // Only the last part of a mapped volume.
  let i = name?.lastIndexOf('/') ?? -1;
  return (i < 0 ? name : name?.substring(i + 1)) ?? '';
};

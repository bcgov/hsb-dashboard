export const groupBy = <InT, OutT = InT>(
  items: InT[],
  predicateKey: (item: InT) => string,
  predicateValue: (item: InT) => OutT = (i) => i as any,
) => {
  return items.reduce<{ [key: string]: OutT[] }>((result, item) => {
    const key = predicateKey(item);
    const group = result[key] || [];
    group.push(predicateValue(item));
    result[key] = group;
    return result;
  }, {});
};

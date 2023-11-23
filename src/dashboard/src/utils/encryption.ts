import Cryptr from 'cryptr';

const secretKey = `${process.env.NEXTAUTH_SECRET}`;
const cryptr = new Cryptr(secretKey);

export const encrypt = (value: string) => {
  return cryptr.encrypt(value);
};

export const decrypt = (value: string) => {
  return cryptr.decrypt(value);
};

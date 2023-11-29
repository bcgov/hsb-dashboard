import type { Config } from 'tailwindcss';
import colors from 'tailwindcss/colors';

const config: Config = {
  content: ['./src/**/*.{js,ts,jsx,tsx,mdx}'],
  theme: {
    extend: {
      backgroundImage: {
        'gradient-radial': 'radial-gradient(var(--tw-gradient-stops))',
        'gradient-conic': 'conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))',
      },
      colors: {
        blue: {
          DEFAULT: '#003366',
          '50': '#EBF5FF',
          '100': '#D6EBFF',
          '200': '#ADD6FF',
          '300': '#85C2FF',
          '400': '#5CADFF',
          '500': '#3399FF',
          '600': '#0A85FF',
          '700': '#0070E0',
          '800': '#0066CC',
          '900': '#0052A3',
          '950': '#003D7A',
          '1000': '#003366',
          '1010': '#001F3D',
          '1020': '#000A14',
        },
        gray: {
          DEFAULT: '#F2F2F2',
          '50': '#F2F2F2',
          '100': '#EBEBEB',
          '200': '#D6D6D6',
          '300': '#C2C2C2',
          '400': '#ADADAD',
          '500': '#999999',
          '600': '#858585',
          '700': '#707070',
          '800': '#5C5C5C',
          '900': '#474747',
          '950': '#3D3D3D',
        },
        yellow: {
          DEFAULT: '#FCBA19',
          ...colors.yellow,
        },
        green: {
          DEFAULT: colors.green['500'],
          ...colors.green,
        },
        red: {
          DEFAULT: colors.red['500'],
          ...colors.red,
        },
      },
    },
  },
  plugins: [],
};
export default config;

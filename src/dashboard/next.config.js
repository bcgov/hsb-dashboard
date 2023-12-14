/** @type {import('next').NextConfig} */
const path = require('path');

const nextConfig = {
    reactStrictMode: true,
    swcMinify: true,
    experimental: {},
    sassOptions: {
      includePaths: [path.join(__dirname, 'styles')],
    },
    // images: {
    //   imageSizes: [16, 32, 48, 64, 96, 128, 256],
    //   domains: ["images.unsplash.com"],
    //   minimumCacheTTL: 3600,
    //   formats: ["image/webp"],
    // },
    webpack: (config, context) => {
      // Enable polling based on env variable being set
      if(process.env.NEXT_WEBPACK_USEPOLLING) {
        config.watchOptions = {
          poll: 500,
          aggregateTimeout: 300
        }
      }
      return config
    },
}

module.exports = nextConfig

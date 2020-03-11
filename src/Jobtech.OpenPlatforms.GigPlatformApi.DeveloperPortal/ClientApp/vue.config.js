module.exports = {
  //baseUrl: '/',
  publicPath: '/',
  assetsDir: 'dist',
  outputDir: 'build',
  // devServer: {
  //   open: process.platform === 'darwin',
  //   host: '0.0.0.0',
  //   port: 8080, // CHANGE YOUR PORT HERE!
  //   https: true,
  //   hotOnly: false,
  // },
  css: {
    loaderOptions: {
      sass: {
        data: `@import "@/assets/scss/_imports/_importMaster.scss";`
      }
    }
  }
};

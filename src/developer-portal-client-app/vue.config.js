module.exports = {
  //baseUrl: '/',
  publicPath: '/',
  assetsDir: 'dist',
  outputDir: 'build',
  devServer: {
    //open: process.platform === 'darwin',
    host: 'localhost',
    port: 5001, // CHANGE YOUR PORT HERE!
    https: true,
    //hotOnly: false,
  },
  css: {
    loaderOptions: {
      sass: {
        data: `@import "@/assets/scss/_imports/_importMaster.scss";`
      }
    }
  }
};

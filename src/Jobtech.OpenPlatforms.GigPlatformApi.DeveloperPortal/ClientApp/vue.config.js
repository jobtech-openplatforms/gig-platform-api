module.exports = {
  //baseUrl: '/',
  publicPath: '/',
  assetsDir: 'dist',
  outputDir: 'build',
  css: {
    loaderOptions: {
      sass: {
        data: `@import "@/assets/scss/_imports/_importMaster.scss";`
      }
    }
  }
};

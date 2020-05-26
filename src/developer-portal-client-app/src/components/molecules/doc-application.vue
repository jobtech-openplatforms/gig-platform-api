<template lang="pug">
  .box
      h3.color-import Application
      .frame-wrapper
        p Let your users retrieve information from the platforms they have experience with to your application. Please note that approval is required.
          router-link.color-import(to="/application-documentation")  Read more
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
@Component({})
export default class DocApplication extends Vue {
}
</script>

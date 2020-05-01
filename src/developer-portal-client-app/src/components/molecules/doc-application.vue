<template lang="pug">
  .box
      h3.color-import Application
      .frame-wrapper
        p Get access to your user's data or let them access their data from other Open Platforms partners.
          router-link.color-import(to="/application-documentation")  Read more
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
@Component({})
export default class DocApplication extends Vue {
}
</script>

<template lang="pug">
  //- div
    //- h3.color-export Platform
  .frame-wrapper
    p.
      Make sure your users can access their experience / reputation
      data on your platform.
      #[router-link.color-export(to="/platform-documentation")  Read more]
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
@Component({})
export default class DocPlatform extends Vue {
}
</script>

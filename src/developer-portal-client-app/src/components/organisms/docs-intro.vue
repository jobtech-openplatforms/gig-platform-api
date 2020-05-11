<template lang="pug">
  .flex-wrapper
    DocPlatform
    DocApplication
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'
import DocPlatform from '@/components/molecules/doc-platform.vue'
import DocApplication from '@/components/molecules/doc-application.vue'

@Component({
  components: {
    DocPlatform,
    DocApplication
  }
})
export default class DocsIntro extends Vue {
}
</script>

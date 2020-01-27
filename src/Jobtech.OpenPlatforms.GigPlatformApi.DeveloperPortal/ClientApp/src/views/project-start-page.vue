<template lang="pug">
  div.home.project-page.center(v-if="currentProject")
    div(style="height:100%;display:flex;flex-direction:column;")
      h1.center Nice! Now, what do you want to do?
      .flex-wrapper

        .box
          h4 Connect your platform
          .frame-wrapper
            .frame
              p Make sure your users can access their experience / reputation data on your platform.
              .buttons.mt-4
                router-link.btn.btn-export.btn-small(to="/share-user-data") Add platform
        .box
          h4 Register your application
          .frame-wrapper
            .frame
              p Get access to your user's data or let them access their data from other Open Platforms partners.
              .buttons.mt-4
                router-link.btn.btn-import.btn-small(to="/integrate-user-data") Add application

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapGetters } from 'vuex'

@Component({
  computed: {
    ...mapGetters('projects', ['currentProject'])
  }
})
export default class ProjectStartPage extends Vue {}
</script>

<style lang="scss">
</style>

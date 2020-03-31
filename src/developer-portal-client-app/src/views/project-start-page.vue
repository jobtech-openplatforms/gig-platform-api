<template lang="pug">
  div.home.project-page.center(v-if="currentProject")
    div(style="height:100%;display:flex;flex-direction:column;")
      h1 Nice! Now, what do you want to do?
      p Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent mattis vel neque non laoreet. Ut tempor, est sit amet dapibus molestie, purus quam imperdiet tellus, eget gravida sapien nunc vel quam. Proin et nunc augue. Nullam accumsan massa nec mauris ultricies commodo. Morbi quis finibus dolor. Vivamus vestibulum vel enim at lacinia. Aliquam efficitur, orci eu finibus blandit, eros neque porta justo, nec consectetur nisl velit non dui. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
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
      p Suspendisse bibendum arcu a venenatis venenatis. Donec tincidunt justo consectetur rhoncus luctus. Nulla varius hendrerit sapien eget sagittis. Aliquam vehicula enim ac erat pellentesque mattis. Phasellus dignissim augue eget lectus cursus, quis accumsan elit lacinia. Nam blandit at urna sed posuere. Nam venenatis ipsum lacus, nec consequat nunc dictum ac. In non tempus velit. Mauris quis maximus ipsum, sit amet porttitor ex. Etiam sit amet dui consequat, ultricies sem sed, hendrerit elit.

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

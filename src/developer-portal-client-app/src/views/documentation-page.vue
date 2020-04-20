<template lang="pug">
  div.info-page
    .card
      h1 Open Platforms Technical Documentation
      p Technical documentation for integrating with Open Platforms.
      p For a more general overview, see #[router-link(to="/info") info].
      h2 Overview
      p.
        Open Platforms has two types of integrations - #[em.color-export Platform]
        and #[em.color-import Application].
      p.
        In our terminology, a #[em.color-export Platform] is a service that 
        allows gig workers (freelancers, etc.) to do work through 'gigs' 
        (jobs, projects, etc.) and gain experience and reputation. Platforms
        can integrate with Open Platforms and allow their gig workers to
        include their experience, reputation and earnings to be shared 
        with applications for innovative use.

      p.
        An #[em.color-import Application] is a service that allows gig workers
        to integrate their experience, reputation and earnings data from one
        or more platforms into their account with the service.

      h2 Platform integration
      h2 Application integration

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({
})
export default class DocumentationPage extends Vue {}
</script>


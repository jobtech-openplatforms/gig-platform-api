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
      p.
        The #[em.color-export Platform] integration is for you if your service
        serves as a platform for gig workers to perform gigs for customers.
        Preferably including earnings per gig and reputation data like reviews
        and ratings.
      p.
        To integrate with Open Platforms, you will have to create an SSL secured URL endpoint
        that is publicly accessible for the Open Platforms system to retrieve data
        for the individual gig worker from.
      h3 How it works
      h3 TEST mode and LIVE mode
      p.
        The Open Platforms development portal includes two modes for access - TEST mode
        for development and testing, and LIVE mode for live access.
      h2 Application integration
      p.
        The #[em.color-import Application] integration is for services who want to import
        gig data (performed gigs, earnings, reputation, etc.) from one or more platforms.
      h3 How it works

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({
})
export default class DocumentationPage extends Vue {}
</script>


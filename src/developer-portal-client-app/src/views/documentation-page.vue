<template lang="pug">
  div.info-page
    div
      h1 Open Platforms Technical Documentation
      p This is the technical documentation for integrating with Open Platforms. For a more general overview, see #[router-link(to="/info") info].
      h2 Overview
      p.
        Open Platforms has two types of integrations - #[em.color-export Platform]
        and #[em.color-import Application].
      p.
        In our terminology, a #[em.color-export Platform] is a service that
        allows gig workers (freelancers, etc.) to do work through 'gigs'
        (jobs, projects, etc.) and gain experience and reputation. Platforms
        can integrate with Open Platforms and allow users to
        access their experience and reputation data
        in applications.

      p.
        An #[em.color-import Application] is a service that allows users
        to retrieve their gig reputation data from platforms they have experience with.
        After user consent this data will be shared with the application for innovative
        use - for instance better matching with gigs and customers.

    hr.spacious
    div
      h2 DEV mode and LIVE mode
      p.
        The Open Platforms development portal includes two modes for access - #[strong DEV mode]
        for development and testing, and #[strong LIVE mode] for live access.

      p.
        We recommend that you use DEV mode for local development
        with the Open Platforms API.
      p(v-if="!devMode") You are currently working in #[strong.live-text LIVE mode].
      p(v-if="devMode") You are currently working in #[strong.dev-text DEV mode].

    .card.mb-4
      h3 Differences between LIVE and DEV
      ul
        li.
          Switching between modes allows for different settings
          between development and testing scenarios and production
          scenarios.
        li.
          The project name can only be changed in LIVE mode.
        li.
          Functions like #[em Go live] do not work in DEV mode.
        li.
          Test functions for platform and application in both
          LIVE and DEV mode.

    h3 Recommendations

    p.
      For local development, we recommend using #[a(href="https://ngrok.com/" target="_blank") ngrok] to forward requests to your local development machine.

    hr.spacious
    div
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
      p #[router-link.color-export(to="/platform-documentation") Full platform documentation]

    hr.spacious
    div
      h2 Application integration
      p.
        The #[em.color-import Application] integration is for services who want to import
        gig data (performed gigs, earnings, reputation, etc.) from one or more platforms.
      p #[router-link.color-import(to="/application-documentation") Full application documentation]

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState } from 'vuex'

@Component({
  computed: {
    ...mapState('projects', ['devMode'])
  },
})
export default class DocumentationPage extends Vue {}
</script>


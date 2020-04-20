<template lang="pug">
  div.info-page
    .card
      h1 Open Platforms Overview
      p.
        Open Platforms is provided for data mobility for gig workers
        and creating a foundation for innovation in matching platforms
        and gigs with gig workers.
      p.
        The purpose for Open Platforms is to provide the infrastructure
        for innovation in the gig economy.
      h2 Two perspectives: Platform and Application
      p.
        In Open Platforms, #[strong platforms] provide data for individual #[strong gig workers]
        through integration with #[strong applications]. In other words, platforms make 
        data available for each authenticated gig worker to be retrieved by an application.

</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({
})
export default class InfoPage extends Vue {}
</script>


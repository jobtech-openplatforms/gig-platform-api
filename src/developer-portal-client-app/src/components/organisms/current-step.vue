<template lang="pug">
    .current-step.small
      .step(v-if="step == 'platformDataUrlIncomplete'")
        // h2 Next up!
        span.
          #[router-link.color-export(to="/platform-test") Fill out the data url]
          for retrieving data for a user on your platform.
      .step(v-if="step == 'currentProjectIncomplete'")
        span Fill out the details for the project to be able to publish it with Open Platforms
      .step(v-if="step == 'testModeIncomplete'")
        span Just one more thing: Fill out the details for the project to finish the project!
      .step(v-if="step == 'testModeComplete'")
        h2 Dev mode Complete!
        p Seems all settings are complete for this project in Dev mode.
        p.
          Deploy your solution and head over to LIVE mode and update
          the settings to publish on Open Platforms.
      .step(v-if="step == 'liveModeComplete'")
        h2 Congratulations!
        p Seems you've completed all settings on the project.

</template>

<script lang="ts">
    import { Component, Vue, Prop } from 'vue-property-decorator'

    @Component
    export default class CurrentStep extends Vue {

        @Prop({ type: String, required: true })
        step!: string

        constructor()
        {
            super()

            // console.log(this.step)
        }
    }
</script>

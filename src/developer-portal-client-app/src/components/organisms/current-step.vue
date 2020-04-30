<template lang="pug">
    .current-step
      .step(v-if="step == 'platformDataUrlIncomplete'")
        // h2 Next up!
        p.
          #[router-link(to="/platform-test") Fill out the data url]
          for retrieving data for a user on your platform.
      .step(v-if="step == 'currentProjectIncomplete'")
        p Fill out the details for the project to be able to publish it with Open Platforms
      .step(v-if="step == 'testModeIncomplete'")
        p Just one more thing: Fill out the details for the project to finish the project!
      .step(v-if="step == 'testModeComplete'")
        h2 Test Mode Complete!
        p Seems all settings are complete for this project in Test Mode.
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

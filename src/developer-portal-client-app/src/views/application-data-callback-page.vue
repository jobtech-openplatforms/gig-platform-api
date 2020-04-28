<template lang="pug">
    .application-data-callback
        pre
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { mapState, mapActions, mapGetters } from 'vuex'

@Component({})
export default class ApplicationDataCallbackPage extends Vue {
    goto(refName) {
      const el = document.querySelector(refName)
      el && el.scrollIntoView({behavior: 'smooth', block: 'center'})
    }
}
</script>

<template>
  <button
    type="button"
    @click="onClick()"
    class="btn m-2 shadow-sm"
    :style="{backgroundColor:backgroundColor}"
  >
    <div class="text-white text-center my-2">
      <p class="m-0">
        <font-awesome-icon :icon="icon" style="font-size: 25px" />
      </p>
      <p class="m-0 text-break text-wrap">{{text}}</p>
    </div>
  </button>
</template>

<script>
export default {
  name: "CustomBtn",
  props: {
    text: String,
    icon: String,
    backgroundColor: String,
  },
  methods: {
    onClick() {
      this.$emit("onClick", this.text);
    },
  }
};
</script>


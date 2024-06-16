/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts, component.html}"],
  theme: {
    screens: {
      xs: "360px",
      sm: "640px",
      md: "1024px",
      lg: "1440px",
    },
    extend: {},
  },
  plugins: [],
};

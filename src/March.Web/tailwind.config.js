/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./*.razor",
        "./Features/Server/StaticFiles/*.html",
        "./Features/Server/StaticFiles/*.js",
        "./Features/Client/**/*.razor",
    ],
  darkMode: 'selector',
  theme: {
    extend: {},
  },
  plugins: [],
}


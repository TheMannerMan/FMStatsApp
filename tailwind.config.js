/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './FMStatsApp/Pages/**/*.{cshtml,razor,html}',
    './FMStatsApp/Views/**/*.{cshtml,razor,html}',
    './FMStatsApp/Pages/Shared/**/*.{cshtml,razor,html}',
    './FMStatsApp/wwwroot/js/**/*.{js,ts}'
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#eef9ff',
          100: '#d9f0ff',
          200: '#b5e3ff',
          300: '#84d2ff',
          400: '#47b8ff',
          500: '#199bfa',
          600: '#0679d7',
          700: '#045fa9',
          800: '#074f87',
          900: '#0c426f'
        }
      }
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography')
  ]
}

# Getting Started

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run prettier`

Run prettier formatting of `./src/**/*.{ts,tsx}` files. See `.prettierrc` for configuration.

### `npm run api`

Runs code generation for OpenAPI 2.0 documentation. See the `openapi-config.json` file for configuration. `schemaFile` can be either a remote URL or local file relative to the project root.

## Dependencies

### @mui/material

The project uses material ui for the most commonly used components. General styling (colors, fonts, shadows, ...) should be put in `theme/theme.ts` or its belonging files.

### react-redux and @reduxjs/toolkit

The project is set up with redux to support auto generated rtk-query. See https://redux-toolkit.js.org/rtk-query/overview and https://redux-toolkit.js.org/rtk-query/usage/code-generation.

### react-hook-form

Form state and validation is managed through `react-hook-form` where simple React state just doesn't cut it.

### react-i18next

To support i18n we use `react-i18next`. It gets the languages throughout the application from the json locales in the public directory.
JS Intl is used for date and time formatting.

## Project structure

- `/src`

  - `/api`
    ```
    Contains all API related files such as rtk query generated files, API caching and request authorization headers.
    ```
  - `/assets`
    ```
    Contains un-compiled assets such as fonts, images and svg icons.
    ```
  - `/auth`
    ```
    Javascript configuration and auth provider.
    ```
  - `/components`

    ```
    React components directory. Follow kebab-case naming convention for component files, while keeping component names PascalCase. Name tests `component-name.spec.tsx` next to the component file. Re-export relevant components and types from component directory. Heavily inspired by https://charles-stover.medium.com/optimal-file-structure-for-react-applications-f3e35ad0a145.

    Example:

    /project-select
      project-select.spec.tsx
      project-select.tsx      // exports ProjectSelect component
      index.ts             // re-exports ProjectSelect to allow imports from /project-select directly
    ```

  - `/layouts`
    ```
    Custom component wrappers that adds shared content around pages, i.e. headers, navbar, footer etc.
    ```
  - `/pages`
    ```
    Entry points from routing. These are simple components that merely pulls data and render components. Components under pages are written in PascalCase.
    ```
  - `/shared`
    ```
    Custom providers and boundaries that spans the entire application such as routing, dialogs and error boundary.
    ```
  - `/store`
    ```
    Redux store setup. This is where we wire up the generated api slice with redux.
    ```
  - `/theme`
    ```
    Material UI theme files.
    ```
  - `/translations`
    ```
    I18n provider, custom formats and raw translation files.
    ```
  - `/utils`
    ```
    Simple helper functions.
    ```

## Testing

Jest is setup with a standard configuration from `create-react-app` and extended with `@testing-library/react` and `@testing-library/react-hooks`.

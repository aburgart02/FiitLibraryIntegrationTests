module.exports = {
  env: {
    browser: true,
    es6: true,
    node: true,
    mocha: true,
    jest: true,
  },
  globals: {
    Atomics: "readonly",
    SharedArrayBuffer: "readonly",
  },
  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:react/recommended",
    "plugin:storybook/recommended"
  ],
  parser: "@typescript-eslint/parser",
  parserOptions: {
    ecmaFeatures: {
      jsx: true,
    },
    ecmaVersion: 2018,
    sourceType: "module",
  },
  plugins: [
    "react",
    "@typescript-eslint",
  ],
  settings: {
    react: {
      version: "detect",
    },
  },
  rules: {
    // TODO: изменить на "error" после фикса
    "comma-dangle": [
      "warn",
      "always-multiline",
    ],
    // TODO: изменить на "error" после фикса
    // Это правило одно для деструктуризации в импортах и в остальном коде. Решить, нужны ли пробелы внутри фигурных скобок.
    // "object-curly-spacing": [
    //   "warn",
    //   "never",
    // ],
    // TODO: изменить на "error" после фикса + разобраться с индентацией после tbody
    "indent": [
      "warn",
      4,
      {
        "SwitchCase": 1,
      },
    ],
    // TODO: изменить на "error" после фикса
    "quotes": [
      "warn",
      "double",
    ],
    // TODO: изменить на "error" после фикса
    "semi": [
      "warn",
      "always",
    ],
    "no-extra-semi": "warn", // TODO: изменить на "error" после фикса
    "no-useless-escape": "warn", // TODO: изменить на "error" после фикса
    "no-irregular-whitespace": "warn", // TODO: изменить на "error" после фикса
    "no-undef": "warn", //TODO: изменить на "error" после фикса
    "@typescript-eslint/prefer-interface": "warn", // TODO: изменить на "error" после фикса
    "@typescript-eslint/no-empty-interface": "warn", // TODO: изменить на "error" после фикса
    "@typescript-eslint/type-annotation-spacing": "warn", //TODO: изменить на "error" после фикса
    "@typescript-eslint/no-use-before-define": "off",

    "no-case-declarations": "off",
    "no-dupe-class-members": "off",
    "react/prop-types": "off",
    "react/no-find-dom-node": "off",
    "react/display-name": "off",
    "@typescript-eslint/explicit-member-accessibility": "off",
    "@typescript-eslint/no-var-requires": "off",
    "@typescript-eslint/array-type": "off",
    "@typescript-eslint/member-delimiter-style": "warn",
    "@typescript-eslint/no-object-literal-type-assertion": "off",
    "@typescript-eslint/indent": "off", // дублирует eslint(indent)
    "@typescript-eslint/interface-name-prefix": "off",
    "@typescript-eslint/no-parameter-properties": "off",
    "@typescript-eslint/explicit-function-return-type": [
      "warn",
      {
        "allowTypedFunctionExpressions": true,
        "allowExpressions": true,
      }
    ]
  },
};

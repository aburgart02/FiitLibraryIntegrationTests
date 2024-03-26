import type { StorybookConfig } from "@storybook/react-webpack5";
import path from "path";
import MiniCssExtractPlugin from "mini-css-extract-plugin";

const config: StorybookConfig = {
  stories: ["../src/storybook/*.stories.@(ts|tsx)"],
  addons: [
    "@storybook/addon-links",
    "@storybook/addon-essentials",
    "@storybook/addon-onboarding",
    "@storybook/addon-interactions",
  ],
  framework: {
    name: "@storybook/react-webpack5",
    options: {},
  },
  docs: {
    autodocs: "tag",
  },
  webpackFinal: async (config) => {
    config.plugins!.push(new MiniCssExtractPlugin());

    config.module!.rules!.push({
      test: /\.less$/,
      use: [MiniCssExtractPlugin.loader, "css-loader", "less-loader"],
    });

    if (config.resolve) {
      config.resolve.alias = {
        ...config.resolve.alias,
        src: path.resolve(__dirname, "../src"),
      };

      config.resolve = {
        ...config.resolve,
        fallback: {
          querystring: require.resolve("querystring-es3"),
        },
      };
    }

    return config;
  },
};
export default config;

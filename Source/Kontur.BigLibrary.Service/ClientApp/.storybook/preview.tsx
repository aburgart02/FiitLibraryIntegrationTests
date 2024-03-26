import type { Preview } from "@storybook/react";
import React from "react";
import { MemoryRouter } from "react-router-dom";
// import "../src/index.less";

const preview: Preview = {
  parameters: {
    actions: { argTypesRegex: "^on[A-Z].*" },
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/,
      },
    },
  },
  decorators: [
    (StoryFn, context) => (
      <MemoryRouter initialEntries={['/']}>
        <StoryFn {...context} />
      </MemoryRouter>
    ),
  ],
};

export default preview;

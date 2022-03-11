import React from "react";
import Bookcard from "../components/Bookcard";

export default {
  title: "Bookcard",
  component: Bookcard,
};

const Template = (args) => {
  <Bookcard {...args} />;
};

export const Default = Template.bind({});
Default.args = {
  name: "book1",
  id: "1324",
};

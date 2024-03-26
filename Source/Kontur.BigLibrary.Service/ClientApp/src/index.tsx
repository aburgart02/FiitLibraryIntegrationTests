import "core-js/stable";
import "regenerator-runtime/runtime";
import * as React from "react";
import {render} from "react-dom";
import {BrowserRouter} from "react-router-dom";
import {App} from "src/App";
import "./index.less";
import 'bootstrap/dist/css/bootstrap.min.css';

render(<BrowserRouter><App/></BrowserRouter>, document.getElementById("root"));

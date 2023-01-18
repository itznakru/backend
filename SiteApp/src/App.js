import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import { Route, BrowserRouter as Router,Switch,Link } from 'react-router-dom';
import Wizard from './wizard/wizard';


export default function App() {
  return (
      <main>
          <Router>
          <Switch>            
              <Route  path="/error"><h3>ERROR HTML</h3></Route>
              <Route  path="/wizard"><Wizard></Wizard></Route>
              <Route  path="/"><h3>main html</h3></Route>
           </Switch>
           </Router>
      </main>
  )
}

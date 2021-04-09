import * as React from 'react';
import { useRef, forwardRef } from 'react';

import { View, Text,StyleSheet, Dimensions,Animated,Button,Keyboard} from 'react-native';
import LoginScreen from './LoginScreen';
import RegisterScreen from './RegisterScreen';
import Header from './Helpers/Header';
import ExamplePage from './ExamplePage';
import DogList from './DogList';
import RegisterNewDog from './RegisterNewDog';

const {width, height} = Dimensions.get("screen")
const speed=350;
const delta=100;
var pos_Left=-delta;
var pos_right=delta;
var moveDirection=1;

const Headers=[
  /*Example page */[{id: "1",title: "logout",},{id: "3",title: "DogList",},{id: "4",title: "Add Dog",}], 
  /*Login page   */[{id: "1",title: "Sign in",},{id: "2",title: "Sign up",}],                                                    
  /*Registe page */[{id: "1",title: "Sign in",},{id: "2",title: "Sign up",}],  
  /*DogList page */[{id: "1",title: "logout",},{id: "3",title: "DogList",},{id: "4",title: "Add Dog",}],
  /*Register new dog page */[{id: "1",title: "logout",},{id: "3",title: "DogList",},{id: "4",title: "Add Dog",}],
]

export default class Navigator extends React.Component {
  state={
    token: "",
    id: "",
    switchAnim: new Animated.Value(0),
    fadeAnim: new Animated.Value(1),
    navimMainPanel_pos: 0,

    currentViewID: 1,
    HeaderVisible: true,
    }
    NaviData={
      URL: 'http://10.0.2.2:5000/',
      swtichPage: (pageID) => this.swtichPage(pageID),
      setToken: (token,id,mode) => this.setToken(token,id,mode),
    }
    
    constructor(props)
    {
      super(props);
      this.HeaderRef = React.createRef();
    }
    //Keyboard:
    componentDidMount() {
      this.keyboardDidShowListener = Keyboard.addListener('keyboardDidShow', (event)=>{this.showHeader()});
      this.keyboardDidHideListener = Keyboard.addListener('keyboardDidHide', (event)=>{this.hideHeader()});
    }

    showHeader(){
      if( this.HeaderRef.current!=null)
      this.HeaderRef.current.hide()
    }
    hideHeader(){
      if(this.HeaderRef.current!=null)
      this.HeaderRef.current.show()
    }

fading =() =>
{
  Animated.timing(this.state.fadeAnim,{toValue:  0,duration: speed,useNativeDriver: true}).start(
    ()=>{Animated.timing(this.state.fadeAnim,{toValue:  1,duration: speed,useNativeDriver: true}).start();}
  )
}
leftAnim =(newIndx) =>{
  var pos_1;
  var pos_2;
  if (moveDirection==1)
  {
    pos_1=pos_Left;
    pos_2=pos_right;
  }
  else
  {
    pos_1=pos_right;
    pos_2=pos_Left;
  }
  moveDirection =-moveDirection;
  this.setState({ maxH1: - this.state.maxH1 })
  Animated.timing(this.state.switchAnim,{toValue:  pos_1,duration: speed,useNativeDriver: true}).start(
  ()=>{
    //change View
    this.HeaderRef.current.setList(Headers[newIndx]);
    this.setState({currentViewID: newIndx})
    Animated.timing(this.state.switchAnim,{toValue:  pos_2,duration: 1,useNativeDriver: true}).start(
      () =>{Animated.timing(this.state.switchAnim,{toValue:  0,duration: speed,useNativeDriver: true}).start();}
    );
  })
}

moveLeft= (indx) =>{
  this.fading();
  this.leftAnim(indx);
}

// Functions avaible in every component:
swtichPage= (indx)=>{
  console.log("SWITCH PAGE: "+indx)
  if(indx != this.state.currentViewID)
    this.moveLeft(indx);
}
setToken=(newToken,id,mode="UPDATE")=>{
if(mode=="CLEAR"){
  this.setState({token: ""});
  this.setState({id: ""});
}
else{
  console.log("Setting new token: "+ newToken);
  console.log("Setting new ID: "+ id);
  this.setState({token: newToken});
  this.setState({id: id});
}
}

ViewContent = (indx)=>{
  if(indx==0)
  {
    return (<ExamplePage />);
  }
  else if(indx==1)
  {
    return (<LoginScreen    Navi={this.NaviData}/>);
  }
  else if(indx==2)
  {
    return (<RegisterScreen Navi={this.NaviData}/>);
  }
  else if(indx==3)
  {
    return (<DogList        Navi={this.NaviData} token={this.state.token} id={this.state.id}/>);
  }
  else if(indx==4)
  {
    return (<RegisterNewDog Navi={this.NaviData} token={this.state.token} id={this.state.id}/>);
  }
}
render(){
    const switchAnim={
      transform: [
          {
          translateX:this.state.switchAnim,
          }
      ],
      opacity: this.state.fadeAnim,
  };
    var _headerHeight = 3*height/20;
    return(
      <View>
        
        <Animated.View style={[styles.naviMainPanel,switchAnim]}>
          <View style={styles.pageContainer}>
            {this.ViewContent(this.state.currentViewID)}
          </View>
        </Animated.View>
        <View style={styles.naviHeaderPanel}>
          <Header ref={this.HeaderRef} headerHeight={_headerHeight} headerInput={this.swtichPage}/>
        </View>
      </View>
  )
  }
}

const styles = StyleSheet.create({
  pageContainer: {
    backgroundColor: 'white',
    height: '100%', 
    marginHorizontal: 10,
    margin: 10,
    borderRadius: 80,
    alignContent: 'center',
  },
  naviHeaderPanel: {
    marginTop: '5%',
    height: '10%',
    alignContent: 'center',
    //backgroundColor: 'black',
  },
  naviMainPanel: {
    marginTop: '5%',
    height: '80%',
    alignContent: 'center',
  }
});
import * as React from 'react';
import { useRef, forwardRef } from 'react';

import { View, Text,StyleSheet, Dimensions,Animated,Button,Keyboard} from 'react-native';
import LoginScreen from './LoginScreen';
import RegisterScreen from './RegisterScreen';
import Header from './Helpers/Header';
import ExamplePage from './ExamplePage';
import DogList from './DogList';
import RegisterNewDog from './RegisterNewDog';
import DogDetails from './DogDetails';
import UserHome from './UserHome'
import FoundDog from './FoundDog';
import FoundDog2 from './FoundDog_oldV';
import {Backend_Switch} from './Helpers/Backend'
import LoadingPage from './Helpers/LoadingPage'
import FilteredDogList from './FilteredDogList'
import ShelterList from './shelterList'
import ShelterDetails from './ShelterDetails'

const {width, height} = Dimensions.get("screen")
const speed=250;
const delta=100;
var pos_Left=-delta;
var pos_right=delta;
var moveDirection=1;

const Headers=[
  /*0 Example page */               [{id: "1",title: "logout",},  {id: "3",title: "DogList",},  {id: "4",title: "Add Dog",}], 
  /*1 Login page   */               [{id: "1",title: "Sign in",}, {id: "2",title: "Sign up",}],                                                    
  /*2 Registe page */               [{id: "1",title: "Sign in",}, {id: "2",title: "Sign up",}],  
  /*3 DogList page */               [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "4",title: "Add Dog",}],
  /*4 Register new dog page */      [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "7",title: "FoundDog",},{id: "9",title: "Shelters",}],
  /*5 DogDetailed page */           [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "7",title: "FoundDog",},{id: "4",title: "Add Dog",}],
  /*6 User Home page */             [{id: "1",title: "logout",},  {id: "7",title: "FoundDog",}, {id: "4",title: "Add Dog",},{id: "9",title: "Shelters",}],
  /*7 Found Dog Page */             [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "7",title: "FoundDog",},{id: "9",title: "Shelters",}],
  /*8 Filtered Dog List */          [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "7",title: "FoundDog",},{id: "9",title: "Shelters",}],
  /*9 Shelter List */               [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "7",title: "FoundDog",}],
  /*10 ShelterDetailed page */      [{id: "1",title: "logout",},  {id: "6",title: "User",},     {id: "7",title: "FoundDog",},{id: "4",title: "Add Dog",}],

]

export default class Navigator extends React.Component {
  state={
    token: "",
    id: "",
    BackendAvaible: true,
    switchAnim: new Animated.Value(0),
    fadeAnim: new Animated.Value(1),
    loadAnim: new Animated.Value(0),
    navimMainPanel_pos: 0,

    currentViewItem: null,
    currentViewID: 1,
    HeaderVisible: true,
    }
    NaviData={
      URL: 'http://10.0.2.2:5000/',
      swtichPage: (pageID,item) => this.swtichPage(pageID,item),
      setToken: (token,id,mode) => this.setToken(token,id,mode),
      RunOnBackend: (fun,data) => this.RunOnBackend(fun,data),
    }
    
    constructor(props)
    {
      super(props);
      this.HeaderRef = React.createRef();
      this.LoadingRef = React.createRef();
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

loadingSwitch =(mode) =>
{
  if (mode==true)
  {
    this.LoadingRef.current.runAnim()
    Animated.timing(this.state.loadAnim,{toValue:  1,duration: speed,useNativeDriver: true}).start()
  }
  else
  {
    Animated.timing(this.state.loadAnim,{toValue:  0,duration: speed,useNativeDriver: true}).start()
  }
}

leftAnim =(newIndx,item) =>{
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
    this.setState({currentViewItem: item})
    this.setState({currentViewID: newIndx})
    Animated.timing(this.state.switchAnim,{toValue:  pos_2,duration: 1,useNativeDriver: true}).start(
      () =>{Animated.timing(this.state.switchAnim,{toValue:  0,duration: speed,useNativeDriver: true}).start();}
    );
  })
}

moveLeft= (indx,item) =>{
  this.fading();
  this.leftAnim(indx,item);
}

// Functions avaible in every component:
swtichPage= (indx,item)=>{
  //console.log("SWITCH PAGE: "+indx + " ITEM: " + item)
  if(indx != this.state.currentViewID)
    this.moveLeft(indx,item);
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


RunOnBackend = (fun,data)=>
{
  if(this.state.BackendAvaible==false) return new Promise((resolve,reject)=>{reject("Backend is busy")})
  this.setState({BackendAvaible: true});
  console.log("------------- RunOnBackend started")
  this.loadingSwitch(true)
  return new Promise((resolve,reject)=>
    {
      Backend_Switch(fun,data,this.state.token,this.state.id).then(
        (x)=>{
          console.log("RunOnBackend Finished")
          this.loadingSwitch(false)
          this.setState({BackendAvaible: true});
          resolve(x);
          }
        )
        .catch(
          (x)=>{
            console.log("RunOnBackend Rejected")
            this.loadingSwitch(false)
            this.setState({BackendAvaible: true});
            reject(x)
          }
        )
    });
}

ViewContent = (indx,item)=>{
  //console.log("ITEM: " + item);
  if(indx==0)
  {
    return (<FoundDog2 Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>);
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
  else if(indx==5)
  {
    return (<DogDetails Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>);
  }
  else if(indx==6)
  {
    return (<UserHome Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>)
  }
  else if(indx==7)
  {
    return (<FoundDog Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>)
  }
  else if(indx==8)
  {
    return (<FilteredDogList Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>)
  }
  else if(indx==9)
  {
    return (<ShelterList Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>)
  }
  else if(indx==10)
  {
    return (<ShelterDetails Navi={this.NaviData} token={this.state.token} id={this.state.id} item={item}/>)
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
  const loadAnim={
   // opacity: 1,
    opacity: this.state.loadAnim,
  }
    var _headerHeight = 3*height/20;
    return(
      <View>
        
        <Animated.View style={[styles.naviMainPanel,switchAnim]}>
          <View style={styles.pageContainer}>
            {this.ViewContent(this.state.currentViewID, this.state.currentViewItem)}
            <Animated.View style={[{marginTop: -0.8*width},loadAnim]}>
                <LoadingPage ref={this.LoadingRef}/>
            </Animated.View>
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
  },
  naviMainPanel: {
    marginTop: '13%',
    height: '80%',
    alignContent: 'center',
  }
});

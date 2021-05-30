import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity, Image,ScrollView,SafeAreaView,Animated } from 'react-native';
import found from '../Assets/found.png'
import serach from '../Assets/search.png'
import PicturePage from './InfoPages/PicturePage.js'
import LocationPage from './InfoPages/LocationPage.js'
import BreedPage from './InfoPages/BreedPage.js'
import AgePage from './InfoPages/AgePage.js'
import SizePage from './InfoPages/SizePage.js';
import ColorPage from './InfoPages/ColorPage.js';
import TimePage from './InfoPages/TimePage.js';

const {width, height} = Dimensions.get("screen")
const speed=300;
const delta=100;
var pos_Left=-delta;
var pos_right=delta;
var moveDirection=1;

export default class FoundDog extends React.Component {
    state={
        // Copied from other page most of this is not usefull
        image: null,
        breed: "",


        ageFrom: "",
        ageTo: "",
        size: "",
        color: "",
        name: "",
        locationCity: "",
        locationDistrict: "",
        dateLostBefore: "",
        dateLostAfter: "",

        //For ui:
        index: 1,
        switchAnim: new Animated.Value(0),
        fadeAnim: new Animated.Value(1),
    }

    ChildrenRef={
        moveToNext: () => this.moveToNext(),
        SetData: (Data,value) => this.SetData(Data,value),
        setBreed: (x) => this.setBreed(x),
        setPicture: (p) => this.setPicture(p),
      }

      setPicture=(p)=>{
        console.log("picture is set: "+ p)
        this.setState({image: p})
      }
      setBreed=(x)=>{
        console.log("Breed is set: "+ x)
        this.setState({breed: x})
      }


    moveToNext =()=>
    {
        this.fading();
        this.moving();
    }
    fading =() =>
    {
        Animated.timing(this.state.fadeAnim,{toValue:  0,duration: speed,useNativeDriver: true}).start(
            ()=>{Animated.timing(this.state.fadeAnim,{toValue:  1,duration: speed,useNativeDriver: true}).start();}
        )
    }
    moving =() =>{
        console.log(" --- moveToNext")
        Animated.timing(this.state.switchAnim,{toValue:  pos_Left,duration: speed,useNativeDriver: true}).start(
        ()=>{
          //change View
          this.setState({index: this.state.index+1})
          Animated.timing(this.state.switchAnim,{toValue:  pos_right,duration: 1,useNativeDriver: true}).start(
            () =>{Animated.timing(this.state.switchAnim,{toValue:  0,duration: speed,useNativeDriver: true}).start();}
          );
        })
      }

    SetData = (Data,value)=>{

    }

    SearchButton=()=>{
        data ={
          breed: this.state.breed,
          ageFrom: this.state.ageFrom,
          ageTo: this.state.ageTo,
          size: this.state.size,
          color: this.state.color,
          name: this.state.name,
          locationCity: this.state.locationCity,
          locationDistrict: this.state.locationDistrict,
          dateLostBefore: this.state.dateLostBefore,
          dateLostAfter: this.state.dateLostAfter,
        }
        console.log("Wysyłam dane: "+ data);
        this.props.Navi.swtichPage(8,data);
      }

    ViewContent = ()=>
    {
        if(this.state.index==1)
        {
          return (<PicturePage ParentRef={this.ChildrenRef}/>);
        }
        else if(this.state.index==2)
        {
          return (<LocationPage ParentRef={this.ChildrenRef}/>);
        }
        else if(this.state.index==3)
        {
          return (<TimePage ParentRef={this.ChildrenRef}/>);
        }
        else if(this.state.index==4)
        {
          return (<BreedPage ParentRef={this.ChildrenRef}/>);
        }
        else if(this.state.index==5)
        {
          return (<AgePage ParentRef={this.ChildrenRef}/>);
        }
        else if(this.state.index==6)
        {
          return (<SizePage ParentRef={this.ChildrenRef}/>);
        }
        else if(this.state.index==7)
        {
          return (<ColorPage ParentRef={this.ChildrenRef}/>);
        } 
    }

  render(){
    const switchAnim={
        transform: [
            {
            translateX:this.state.switchAnim,
            },
        ],
        opacity: this.state.fadeAnim,
    }
    return(
        <View style={styles.content}>
            <View style={[{flexDirection: 'row', width: 150, margin: 20}]}>
                <Image source={found} style={[styles.Icon,{width: 100, height:100}]}/>
                <Text  style={[{ width: '100%', fontSize: 25, fontWeight: 'bold', textAlignVertical: 'center', color: '#99481f'}]}> What kind of dog have you seen ?</Text>
            </View> 
            <Animated.View style={[styles.PageHolder,switchAnim]}>
                {this.ViewContent()}
            </Animated.View>
        </View>
  )
  }
}


const styles = StyleSheet.create({
    Center:{
        marginLeft: 'auto', 
        marginRight: 'auto',
        alignSelf: 'center',
        },
    content: {
        marginHorizontal: 30,
        height: '100%',
        width: '80%',
        alignSelf: 'center',
        justifyContent: 'center',
        marginVertical: 'auto',
    },
    PageHolder:{
        height: '60%'
    }
});

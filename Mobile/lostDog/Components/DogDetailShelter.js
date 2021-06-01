import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image,SafeAreaView, ScrollView } from 'react-native';
import CalendarIcon from '../Assets/calendar.png';
import ClockIcon from '../Assets/clock.png';
import GpsIcon from '../Assets/gps.png';
import NormalDog from '../Assets/smallDog.png';
import Mark from '../Assets/mark.png';
import Ear from '../Assets/ear.png';
import Age from '../Assets/age.png';
import Size from '../Assets/size.png';
import Hair from '../Assets/hair.png';
import Tail from '../Assets/tail.png';
import Brain from '../Assets/brain.png';
import SkipIcon from '../Assets/skip.png';
import success from '../Assets/success.png';
import notify from '../Assets/notify.png';
import ShelterIcon from '../Assets/animal-shelter.png'

const {width, height} = Dimensions.get("screen")
const stc = require('string-to-color');


export default class DogDetailShelter extends React.Component {

  state={
    data: null,
  }
  constructor(props) {
    super(props);
    console.log(this.props.item.behaviors)
   }

   componentDidMount(){
    this.setState({data: this.props.item})
    console.log(this.state.data)
  }

  toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }

  PutDataFailed=()=>{
    console.log("Set data as found Failed");
  }


  Date=(x)=>{
    if(this.validate(x)) return (<View></View>)
    return(
     <View style={styles.Info}>
         <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={CalendarIcon} />
         <Text style={styles.InfoText} >Lost on {x}</Text>
     </View>
    )
}
Time=(x)=>{
 if(this.validate(x)) return (<View></View>)
 return(
  <View style={styles.Info}>
      <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={ClockIcon} />
      <Text style={styles.InfoText} >Around time: {x}</Text>
  </View>
 )
 }
 LocationCity=(x)=>{
     if(this.validate(x)) return (<View></View>)
     return(
     <View style={styles.Info}>
         <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={GpsIcon} />
         <Text style={styles.InfoText} >{x}</Text>
     </View>
     )
 }
 LocationDistrict=(x)=>{
     if(this.validate(x)) return (<View></View>)
     return(
     <View style={styles.Info}>
         <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={GpsIcon} />
         <Text style={styles.InfoText} >District: {x}</Text>
     </View>
     )
 }
 OtherInfo=(icon,title,x)=>{
     if(this.validate(x)) return (<View></View>)
     return(
     <View style={styles.Info}>
         <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={icon} />
         <Text style={styles.InfoText} >{title}: {x}</Text>
     </View>
     )
 }
 Color=(x)=>{
     if(this.validate(x)) return (<View></View>)
     const color = stc(x);
     const maxSize=40;
     return(
         <View style={styles.Info}>
             <View style={[styles.InfoIcon,{marginLeft: '5%', backgroundColor: color, borderRadius: 10}]} />
             {
                 x.length>maxSize?
                 <Text style={styles.InfoText} >Color: {x.substring(0,maxSize)}...</Text>:
                 <Text style={styles.InfoText} >Color: {x}</Text>
             }
         </View>
         )
 }

 LongInfo=(icon,title,x)=>{
     if(this.validate(x)) return (<View></View>)
     return(
     <View style={[styles.Info,{height: 50}]}>
         <Image style={[styles.InfoIcon,{marginLeft: '5%'}]} source={icon} />
         <Text style={styles.InfoText} >{title}: {x}</Text>
     </View>
     )
 }
 validate=(x)=>{
  return(x=="" || x==null || x=='None')
}

ListOfDogs=()=>{
  this.props.Navi.swtichPage(10,this.state.data.shelter);
}

 
  render(){
    return(
      <View style={styles.content}>
      {
        this.state.data!=null?
        <View>
            <Text style={{fontWeight: 'bold', fontSize: 30}}>{this.state.data.dog.name}</Text>
            <View style={{flexDirection: 'row', height: 0.3*height}}>
              {/*Picture*/
                this.state.data.dog.picture.data!=null? 
                <Image source={{uri: this.toUri(this.state.data.dog.picture)}} style={styles.dogPic}/> 
                : <View/>
              }
              <Image source={ShelterIcon} style={styles.dogPic}/> 
            </View>
            <SafeAreaView style={styles.Scrollcontainer}>
              <ScrollView style={styles.scrollView}>
                <View>
                        {this.LocationCity("In Shelter")}
                        {this.OtherInfo(NormalDog,"Breed",this.state.data.dog.breed)}
                        {this.OtherInfo(Age,"Age",this.state.data.dog.age)}
                        {this.OtherInfo(Size,"Size",this.state.data.dog.size)}
                        {this.OtherInfo(Hair,"Hair",this.state.data.dog.hairLength)}
                        {this.OtherInfo(Ear,"Ears",this.state.data.dog.earsType)}
                        {this.OtherInfo(Tail,"Tail",this.state.data.dog.tailLength)}
                        {this.LongInfo(Mark,"Special mark",this.state.data.dog.specialMark)}
                        {
                            this.state.data.dog.behaviors.map((e, index)=><Text key={index}>{this.LongInfo(Brain,"-Behaviour",e)}</Text>)
                        }
                        {this.Color(this.state.data.dog.color)}
                        <Text style={{fontSize: 30, margin: 20, alignSelf: 'center',color: '#99481f'}}>-</Text>
                </View>
              </ScrollView>
            </SafeAreaView>
        </View>
        :null
      }
      <TouchableOpacity style={[styles.Button,{height: 0.05*height}]} onPress={() => this.ListOfDogs()}>
        <Image style={[styles.ButtonIcon,{marginLeft: '5%', transform: [{ scaleX: -1 }]}]} source={SkipIcon} />
        <Text style={styles.ButtonText} >Back</Text>
      </TouchableOpacity>
      </View>
  )
  }

}


const styles = StyleSheet.create({
    infoText:{
      fontSize: 15,
      margin: 2,
    },
      content: {
        margin: 30,
        alignSelf: 'center',
        justifyContent: 'center',
      },
      Scrollcontainer:{
        width: 0.75*width,
        height: 0.3*height,
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    },
    dogPic:{
      height: 0.15*height,
      width: 0.15*height,
      resizeMode: 'contain',
      aspectRatio: 0.7, 
      borderRadius: 600,
      alignSelf: 'center',
      marginRight: 10,
      
    },
    Info:{
      //backgroundColor: '#feb26d',
      width: width*0.8,
      height: height*0.04,
      flexDirection: 'row',
      alignContent: 'center',
      borderRadius: 15,
  },
  InfoText:{
      marginTop: 'auto',
      marginBottom: 'auto',
      marginLeft: 15,
      fontSize: 15,
      color: '#99481f',
      alignSelf: 'flex-start',
      width: '75%',
  },
  InfoIcon:{
      width: 25,
      height:25,
      alignSelf: 'center',
      tintColor: '#99481f'
  },
  Button:{
    backgroundColor: '#feb26d',
    width: width*0.4,
    height: height*0.1,
    margin: 10,
    marginLeft: 'auto',
    marginRight: 'auto',
    flexDirection: 'row',
    alignContent: 'center',
    borderRadius: 15,
},
ButtonText:{
    marginTop: 'auto',
    marginBottom: 'auto',
    marginHorizontal: 5,
    fontSize: 18,
    color: 'white',
    textAlign: 'center',
    width: '60%',
},
ButtonIcon:{
    width: 35,
    height:35,
    alignSelf: 'center',
},
});

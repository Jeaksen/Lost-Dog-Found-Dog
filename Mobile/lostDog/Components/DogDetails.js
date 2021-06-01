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

const {width, height} = Dimensions.get("screen")
const stc = require('string-to-color');


export default class DogDetails extends React.Component {

  state={
    data: null,
  }
  constructor(props) {
    super(props);
    console.log(this.props.item.behaviors)
   }

   componentDidMount(){
    this.setState({data: this.props.item})
  }

  toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }

  SetDogAsFound=(id)=>{
    //var token = 'Bearer ' + this.props.token 
    console.log("SetDogAsFound" + this.props.token + " id: " + this.state.data.id)
    fetch(this.props.Navi.URL + 'lostdogs/'+this.state.data.id+'/found', {
        method: 'PUT', 
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*',
            'Authorization': this.props.token ,
        },
    })
    .then(response=>{return response.json();})
        .then(responseData => {
          console.log("ADD COMENT RESPONSE DATA:")  
          console.log(responseData)  
          if (responseData.successful==false) {
            console.log("ERROR")  
            console.log(responseData)  
            return null;
            }
            else
            {
              return (responseData.data);
            }
            })
      .catch((x)=>{
        console.log(x)
        reject(null)
        return null;
      })
      .finally(()=>{this.props.Navi.swtichPage(3)})
  }
  PutDataFailed=()=>{
    console.log("Set data as found Failed");
  }

  ButtonRound=(l1,l2,callback)=>{
      return(
          <TouchableOpacity onPress={callback}>
            <View style={{ backgroundColor: 'green', margin: 3, borderRadius: 60, height: 73,width: 73, borderColor: 'green', justifyContent: 'center'}}>
              <View style={{alignSelf: 'center', padding: 5}}>
                  <Text style={{color: 'white', fontSize: 13}}>{l1}</Text>
                  {l2!=null? 
                  <Text style={{color: 'white', fontSize: 13}}>{l2}</Text>:null}
              </View>
            </View>
          </TouchableOpacity>
      )
  }

  getFoundInfo=()=>{
    return(
      <View>
        <Text style={{fontSize: 13, margin: 5}}>
        We are thrilled to hear{'\n'}
        your puppy is found!{'\n'}
        Don't forget to hug{'\n'}
        him from us and remember {'\n'}
        to help others by posting{'\n'}
        info everytime{'\n'}
        you see
        <Text style={{fontWeight: 'bold'}}> Lost Dog</Text>
        </Text>
      </View>
    )
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
         <Text style={styles.InfoText} >In the city: {x}</Text>
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
  this.props.Navi.swtichPage(3,null);
}

showComments=()=>{
  this.props.Navi.swtichPage(13,{dogId: this.state.data.id, backItem: this.state.data});
}
 
  render(){
    return(
      <View style={styles.content}>
      {
        this.state.data!=null?
        <View>
            <Text style={{fontWeight: 'bold', fontSize: 30}}>{this.state.data.name}</Text>
            <View style={{flexDirection: 'row', height: 0.3*height}}>
              {/*Picture*/
                this.state.data.picture.data!=null? 
                <Image source={{uri: this.toUri(this.state.data.picture)}} style={styles.dogPic}/> 
                : <View/>
              }
              {
                this.state.data.isFound==false?
                <View>
                  <TouchableOpacity style={styles.Button} onPress={() => this.SetDogAsFound()}>
                    <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={success} />
                    <Text style={styles.ButtonText} >Set dog as found</Text>
                  </TouchableOpacity>
                  <TouchableOpacity style={styles.Button} onPress={() => this.showComments()}>
                    <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={notify} />
                    <Text style={styles.ButtonText} >Show where he was seen</Text>
                  </TouchableOpacity>
                  {
                    
                  /*
                  {this.ButtonRound('   Set dog ',' as  found ',this.SetDogAsFound)}
                  {this.ButtonRound('Show','Comments',null)}
                  {this.ButtonRound('Love Dog',null,null)}
                  */
                  }
                </View>:
                this.getFoundInfo(this.state.data.isFound)
              }
            </View>
            <SafeAreaView style={styles.Scrollcontainer}>
              <ScrollView style={styles.scrollView}>
                <View>
                        {this.Date(this.state.data.dateLost.split("T")[0])}
                        {this.Time(this.state.data.dateLost.split("T")[1])}
                        {this.LocationCity(this.state.data.location.city)}
                        {this.LocationDistrict(this.state.data.location.district)}
                        {this.OtherInfo(NormalDog,"Breed",this.state.data.breed)}
                        {this.OtherInfo(Age,"Age",this.state.data.age)}
                        {this.OtherInfo(Size,"Size",this.state.data.size)}
                        {this.OtherInfo(Hair,"Hair",this.state.data.hairLength)}
                        {this.OtherInfo(Ear,"Ears",this.state.data.earsType)}
                        {this.OtherInfo(Tail,"Tail",this.state.data.tailLength)}
                        {this.LongInfo(Mark,"Special mark",this.state.data.specialMark)}
                        {
                            this.state.data.behaviors.map((e, index)=><Text key={index}>{this.LongInfo(Brain,"-Behaviour",e)}</Text>)
                        }
                        {this.Color(this.state.data.color)}
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

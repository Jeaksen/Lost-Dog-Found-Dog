import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity ,ScrollView,SafeAreaView,FlatList,Image} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import CommentListItem from './Helpers/CommentListItem';
import SkipIcon from '../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class CommentListPage extends React.Component {
  state={
    info: " normal ",
    CommentList: [],
  }

  getCommentList = ()=>{
      const data={
        dogId: this.props.item.dogId
      }
    this.props.Navi.RunOnBackend("getComment",data).then((responseData)=>{
      //console.log(responseData)
      this.setState({CommentList: responseData.comments});
      console.log("succes list of comments is ready")
      console.log(this.state.CommentList[0].id)
    }).catch((x)=>
        console.log("Comments Error" + (x))
      )
  }

  constructor(props) {
    super(props);
    this.getCommentList();
   }

   back=()=>{
    this.props.Navi.swtichPage(3,null);
   }
  render(){
    return(
        <View style={styles.content}>
            <FlatList
            data={this.state.CommentList.length>0 ? this.state.CommentList : []}
            renderItem={({item}) => <CommentListItem item={item}/>}
            keyExtractor={(item) => item.id.toString()}
           />
           <TouchableOpacity style={styles.Button} onPress={() => this.back()}>
                    <Image style={[styles.ButtonIcon, {marginLeft: '5%', transform: [{ scaleX: -1 }]}]} source={SkipIcon} />
                    <Text style={styles.ButtonText} >Back</Text>
            </TouchableOpacity>
        </View>
  )
  }
}

const styles = StyleSheet.create({
  inputtext: {
    fontSize: 16,
    height: 30,
    width: width*0.5,
    borderColor: '#000000',
    borderWidth: 1,
    borderRadius: 5,
    paddingLeft: 5,
    marginVertical: 10,
  },
  content: {
    marginTop:30,
    margin: 15,
    height: '90%',
    alignSelf: 'center',
    justifyContent: 'center',
  },
  loginButton:{
    marginTop: 20,
    backgroundColor: 'black',
    width: width*0.2,
    height: height*0.05,
    marginLeft: 'auto',
    marginRight: 'auto',
},
logintext:{
    marginTop: 'auto',
    marginBottom: 'auto',
    fontSize: 15,
    color: 'white',
    textAlign: 'center',
},
Title:{
  marginBottom: 50,
  fontSize: 20,
  textAlign: 'center',
  fontWeight: 'bold',
},
Button:{
  backgroundColor: '#feb26d',
  width: width*0.5,
  height: height*0.06,
  margin: 20,
  marginLeft: 'auto',
  marginRight: 'auto',
  flexDirection: 'row',
  alignContent: 'center',
  borderRadius: 15,
},
ButtonText:{
  marginTop: 'auto',
  marginBottom: 'auto',
  fontSize: 15,
  color: 'white',
  textAlign: 'center',
  width: '75%',
},
ButtonIcon:{
  width: 35,
  height:35,
  alignSelf: 'center',
}
});
